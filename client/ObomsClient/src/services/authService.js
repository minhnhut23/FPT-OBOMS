import axios from 'axios';

// Create axios instance with base URL
const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor to include auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Authentication Services
export const authService = {
  // Login with email and password
  login: async (credentials) => {
    const response = await api.post('/auth/login', credentials);
    if (response.data.accessToken) {
      localStorage.setItem('accessToken', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      localStorage.setItem('tokenExpiry', response.data.expiresAt);
    }
    return response.data;
  },

  // Register new user
  register: async (userData) => {
    // Format date to match backend expectations (if needed)
    if (userData.birthDate) {
      const date = new Date(userData.birthDate);
      if (!isNaN(date.getTime())) {
        userData.dateOfBirth = date.toISOString().split('T')[0];
      }
      delete userData.birthDate;
    }

    // Add role if not provided
    if (!userData.role) {
      userData.role = 'Customer';
    }

    // If confirmPassword doesn't exist in userData, create it
    if (!userData.confirmPassword && userData.password) {
      userData.ConfirmPassword = userData.password;
    } else if (userData.confirmPassword) {
      // API expects ConfirmPassword with capital C
      userData.ConfirmPassword = userData.confirmPassword;
      delete userData.confirmPassword;
    }

    const response = await api.post('/auth/register', userData);
    return response.data;
  },

  // Get current user profile
  getProfile: async () => {
    const response = await api.get('/auth/GetProfile');
    return response.data;
  },

  // Forgot password - request OTP
  forgotPassword: async (email) => {
    const response = await api.post('/auth/forgotPassword', { email });
    return response.data;
  },

  // Recover password with OTP
  recoverPassword: async (recoveryData) => {
    // Ensure proper casing for API
    const formattedData = {
      otp: recoveryData.otp,
      email: recoveryData.email,
      NewPassword: recoveryData.newPassword,
      ConfirmPassword: recoveryData.confirmPassword,
    };

    const response = await api.post('/auth/recoverPassword', formattedData);
    return response.data;
  },

  // Change password (requires authentication)
  changePassword: async (passwordData) => {
    // Ensure proper casing for API
    const formattedData = {
      OldPassword: passwordData.oldPassword,
      NewPassword: passwordData.newPassword,
      ConfirmPassword: passwordData.confirmPassword,
    };

    const response = await api.post('/auth/changePassword', formattedData);
    return response.data;
  },

  // Logout user
  logout: async () => {
    try {
      // Call the logout API endpoint if token exists
      if (localStorage.getItem('accessToken')) {
        await api.post('/auth/logout');
      }
    } catch (error) {
      console.error('Logout API error:', error);
    } finally {
      // Always clear local storage regardless of API result
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('tokenExpiry');
    }
  },

  // Helper to check if the token is expired
  isTokenExpired: () => {
    const expiryTime = localStorage.getItem('tokenExpiry');
    if (!expiryTime) return true;

    return new Date(expiryTime) <= new Date();
  },
};

export default authService;
