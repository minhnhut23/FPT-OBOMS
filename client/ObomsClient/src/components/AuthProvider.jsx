import { useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useDispatch } from 'react-redux';
import { authService } from '../services/authService';
import { loginSuccess, loginFailure, logout as logoutAction } from '../store/slices/authSlice';

export const AuthProvider = ({ children }) => {
  const dispatch = useDispatch();
  const accessToken = localStorage.getItem('accessToken');
  const refreshToken = localStorage.getItem('refreshToken');
  const expiresAt = localStorage.getItem('tokenExpiry');

  // Check if user is authenticated on app load
  useQuery({
    queryKey: ['profile'],
    queryFn: () => authService.getProfile(),
    onSuccess: (data) => {
      dispatch(
        loginSuccess({
          user: data,
          accessToken,
          refreshToken,
          expiresAt,
        })
      );
    },
    onError: (error) => {
      console.error('Auth check failed:', error);
      dispatch(loginFailure(error.message));
      // Remove invalid tokens
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('tokenExpiry');
    },
    // Only run if we have a token in localStorage and it's not expired
    enabled: !!accessToken && !authService.isTokenExpired(),
    retry: false,
    refetchOnWindowFocus: false,
  });

  // Setup authentication listener
  useEffect(() => {
    const handleStorageChange = () => {
      const currentToken = localStorage.getItem('accessToken');
      if (!currentToken && accessToken) {
        // Token was removed, log the user out in the app state
        dispatch(logoutAction());
      }
    };

    window.addEventListener('storage', handleStorageChange);
    return () => window.removeEventListener('storage', handleStorageChange);
  }, [dispatch, accessToken]);

  // Check token expiry
  useEffect(() => {
    if (accessToken && authService.isTokenExpired()) {
      // Token is expired, log the user out
      console.log('Token expired, logging out');
      authService.logout();
      dispatch(logoutAction());
    }
  }, [accessToken, dispatch]);

  return children;
};

export default AuthProvider;
