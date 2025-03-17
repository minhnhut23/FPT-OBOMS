import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  user: null,
  accessToken: null,
  refreshToken: null,
  expiresAt: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
  passwordReset: {
    isRequested: false,
    otpSent: false,
    email: null,
    error: null,
  },
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginStart: (state) => {
      state.isLoading = true;
      state.error = null;
    },
    loginSuccess: (state, action) => {
      state.isLoading = false;
      state.isAuthenticated = true;
      state.user = action.payload.user;
      state.accessToken = action.payload.token || action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresAt = action.payload.expiresAt;
      state.error = null;
    },
    loginFailure: (state, action) => {
      state.isLoading = false;
      state.error = action.payload;
    },
    logout: (state) => {
      state.user = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.expiresAt = null;
      state.isAuthenticated = false;
      state.error = null;
      state.passwordReset = initialState.passwordReset;
    },
    updateUser: (state, action) => {
      if (state.user) {
        state.user = { ...state.user, ...action.payload };
      }
    },
    forgotPasswordStart: (state) => {
      state.passwordReset.isRequested = true;
      state.passwordReset.otpSent = false;
      state.passwordReset.error = null;
    },
    forgotPasswordSuccess: (state, action) => {
      state.passwordReset.isRequested = false;
      state.passwordReset.otpSent = true;
      state.passwordReset.email = action.payload.email;
      state.passwordReset.error = null;
    },
    forgotPasswordFailure: (state, action) => {
      state.passwordReset.isRequested = false;
      state.passwordReset.otpSent = false;
      state.passwordReset.error = action.payload;
    },
    resetPasswordState: (state) => {
      state.passwordReset = initialState.passwordReset;
    },
    refreshTokenSuccess: (state, action) => {
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresAt = action.payload.expiresAt;
    },
  },
});

export const {
  loginStart,
  loginSuccess,
  loginFailure,
  logout,
  updateUser,
  forgotPasswordStart,
  forgotPasswordSuccess,
  forgotPasswordFailure,
  resetPasswordState,
  refreshTokenSuccess,
} = authSlice.actions;

export const authReducer = authSlice.reducer;
