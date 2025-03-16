import { useMutation, useQuery } from '@tanstack/react-query';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import {
  loginStart,
  loginSuccess,
  loginFailure,
  logout as logoutAction,
  forgotPasswordStart,
  forgotPasswordSuccess,
  forgotPasswordFailure,
  resetPasswordState,
} from '../store/slices/authSlice';
import { queryClient } from '../lib/react-query';
import toast from 'react-hot-toast';

export const useAuth = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { passwordReset } = useSelector((state) => state.auth);

  // Get current user profile
  const { data: user, isLoading: isLoadingUser } = useQuery({
    queryKey: ['profile'],
    queryFn: () => authService.getProfile(),
    onSuccess: (data) => {
      dispatch(
        loginSuccess({
          user: data,
          accessToken: localStorage.getItem('accessToken'),
          refreshToken: localStorage.getItem('refreshToken'),
          expiresAt: localStorage.getItem('tokenExpiry'),
        })
      );
    },
    onError: () => {
      // If token exists but profile fetch fails, token is likely invalid
      if (localStorage.getItem('accessToken')) {
        authService.logout();
        dispatch(logoutAction());
      }
    },
    // Only run if we have a token
    enabled: !!localStorage.getItem('accessToken') && !authService.isTokenExpired(),
    retry: false,
  });

  // Login mutation
  const {
    mutate: login,
    isPending: isLoggingIn,
    error: loginError,
  } = useMutation({
    mutationFn: (credentials) => {
      dispatch(loginStart());
      return authService.login(credentials);
    },
    onSuccess: (data, variables) => {
      // Extract user data from API response or getProfile
      dispatch(
        loginSuccess({
          user: {
            email: variables.email,
          },
          accessToken: data.accessToken,
          refreshToken: data.refreshToken,
          expiresAt: data.expiresAt,
        })
      );

      toast.success('Login successful!');
      // Invalidate profile query to refetch user data
      queryClient.invalidateQueries({ queryKey: ['profile'] });
      navigate('/');
    },
    onError: (error) => {
      const errorMsg = error.response?.data?.msg || 'Login failed. Please try again.';
      dispatch(loginFailure(errorMsg));
      toast.error(errorMsg);
    },
  });

  // Register mutation
  const {
    mutate: register,
    isPending: isRegistering,
    error: registerError,
  } = useMutation({
    mutationFn: (userData) => authService.register(userData),
    onSuccess: () => {
      toast.success('Registration successful! Please login.');
      navigate('/auth/login');
    },
    onError: (error) => {
      const errorMsg = error.response?.data?.msg || 'Registration failed. Please try again.';
      toast.error(errorMsg);
    },
  });

  // Forgot password mutation
  const {
    mutate: forgotPassword,
    isPending: isRequestingPasswordReset,
    error: forgotPasswordError,
  } = useMutation({
    mutationFn: (email) => {
      dispatch(forgotPasswordStart());
      return authService.forgotPassword(email);
    },
    onSuccess: (_, variables) => {
      dispatch(forgotPasswordSuccess({ email: variables }));
      toast.success('OTP sent to your email. Please check your inbox.');
    },
    onError: (error) => {
      const errorMsg =
        error.response?.data?.msg || 'Failed to request password reset. Please try again.';
      dispatch(forgotPasswordFailure(errorMsg));
      toast.error(errorMsg);
    },
  });

  // Recover password with OTP mutation
  const {
    mutate: recoverPassword,
    isPending: isRecoveringPassword,
    error: recoverPasswordError,
  } = useMutation({
    mutationFn: (recoveryData) => authService.recoverPassword(recoveryData),
    onSuccess: () => {
      dispatch(resetPasswordState());
      toast.success('Password reset successful! Please login with your new password.');
      navigate('/auth/login');
    },
    onError: (error) => {
      const errorMsg = error.response?.data?.msg || 'Failed to reset password. Please try again.';
      toast.error(errorMsg);
    },
  });

  // Change password mutation (for authenticated users)
  const {
    mutate: changePassword,
    isPending: isChangingPassword,
    error: changePasswordError,
  } = useMutation({
    mutationFn: (passwordData) => authService.changePassword(passwordData),
    onSuccess: () => {
      toast.success('Password changed successfully!');
    },
    onError: (error) => {
      const errorMsg = error.response?.data?.msg || 'Failed to change password. Please try again.';
      toast.error(errorMsg);
    },
  });

  // Logout function
  const handleLogout = async () => {
    await authService.logout();
    dispatch(logoutAction());
    // Clear any user-related queries
    queryClient.removeQueries({ queryKey: ['profile'] });
    navigate('/auth/login');
    toast.success('Logged out successfully');
  };

  return {
    user,
    isLoadingUser,
    login,
    isLoggingIn,
    loginError,
    register,
    isRegistering,
    registerError,
    forgotPassword,
    isRequestingPasswordReset,
    forgotPasswordError,
    recoverPassword,
    isRecoveringPassword,
    recoverPasswordError,
    changePassword,
    isChangingPassword,
    changePasswordError,
    logout: handleLogout,
    isAuthenticated: !!user,
    passwordResetState: passwordReset,
  };
};

export default useAuth;
