import { useState } from 'react';
import { motion } from 'framer-motion';
import { EyeIcon, EyeSlashIcon, ArrowLeftIcon } from '@heroicons/react/24/outline';
import { Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import useAuth from '../../hooks/useAuth';
import { useSelector } from 'react-redux';

// Validation schema for requesting OTP
const requestOtpSchema = yup.object({
  email: yup.string().email('Please enter a valid email').required('Email is required'),
});

// Validation schema for password reset
const resetPasswordSchema = yup.object({
  otp: yup.string().required('OTP code is required').length(6, 'OTP must be 6 digits'),
  newPassword: yup
    .string()
    .required('Password is required')
    .min(8, 'Password must be at least 8 characters')
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/,
      'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character'
    ),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('newPassword'), null], 'Passwords must match')
    .required('Confirm password is required'),
});

export default function ForgotPassword() {
  const [showPassword, setShowPassword] = useState(false);
  const {
    forgotPassword,
    isRequestingPasswordReset,
    forgotPasswordError,
    recoverPassword,
    isRecoveringPassword,
    recoverPasswordError,
  } = useAuth();
  const { passwordResetState } = useSelector((state) => state.auth);

  // Form for requesting OTP
  const requestOtpForm = useForm({
    resolver: yupResolver(requestOtpSchema),
    defaultValues: {
      email: '',
    },
  });

  // Form for resetting password
  const resetPasswordForm = useForm({
    resolver: yupResolver(resetPasswordSchema),
    defaultValues: {
      otp: '',
      newPassword: '',
      confirmPassword: '',
    },
  });

  const onRequestOtp = (data) => {
    forgotPassword(data.email);
  };

  const onResetPassword = (data) => {
    recoverPassword({
      email: passwordResetState.email,
      otp: data.otp,
      newPassword: data.newPassword,
      confirmPassword: data.confirmPassword,
    });
  };

  return (
    <div className="min-h-screen bg-gray-100 dark:bg-gray-900 text-gray-900 dark:text-white flex items-center justify-center px-4">
      <div className="absolute inset-0 flex items-center justify-center bg-gradient-to-br from-blue-50 via-indigo-50 to-violet-50 dark:from-black dark:via-black dark:to-black">
        {/* Background Effects */}
        <div className="absolute inset-0 opacity-30">
          <div className="absolute inset-0 bg-gradient-to-r from-white via-slate-100 to-gray-100 dark:from-gray-500 dark:via-blue-700 dark:to-gray-700 animate-gradient" />
          <div className="absolute inset-0 bg-[url('/grid.svg')] opacity-10 dark:opacity-20" />
        </div>

        {/*  Centered Orbs Wrapper */}
        <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-96 h-96 flex justify-center items-center">
          {/* Orb moving from Left to Right */}
          <div className="absolute w-80 h-80 bg-blue-400/30 dark:bg-purple-800/50 rounded-full filter blur-3xl left-to-right" />

          {/* Orb moving from Right to Left */}
          <div className="absolute w-80 h-80 bg-violet-400/30 dark:bg-indigo-800/50 rounded-full filter blur-3xl right-to-left" />
        </div>

        {/* Mesh gradient overlay */}
        <div className="absolute inset-0 bg-gradient-to-t from-white via-white/50 to-white/0 dark:from-[#0F172A] dark:via-transparent dark:to-transparent opacity-60" />
      </div>

      {/* Form Container */}
      <motion.div
        className="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg relative z-10"
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="flex items-center justify-between mb-4">
          <Link to="/auth/login">
            <motion.button
              className="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition"
              whileHover={{ scale: 1.1 }}
              whileTap={{ scale: 0.9 }}
            >
              <ArrowLeftIcon className="w-6 h-6 cursor-pointer" />
            </motion.button>
          </Link>

          <div className="text-3xl font-bold text-center flex-1">Forgot Password</div>

          {/* Empty span to balance layout */}
          <span className="w-6"></span>
        </div>

        {!passwordResetState?.otpSent ? (
          // Request OTP Form
          <form onSubmit={requestOtpForm.handleSubmit(onRequestOtp)} className="mt-6">
            <div className="mb-6">
              <p className="text-gray-600 dark:text-gray-300 text-sm mb-4">
                Enter your email address below and we&apos;ll send you a verification code to reset
                your password.
              </p>
              <label className="block text-gray-700 dark:text-gray-300 mb-2">Email</label>
              <input
                type="email"
                className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                  requestOtpForm.formState.errors.email ? 'border-red-500' : ''
                }`}
                placeholder="Enter your email"
                {...requestOtpForm.register('email')}
              />
              {requestOtpForm.formState.errors.email && (
                <p className="text-red-500 text-sm mt-1">
                  {requestOtpForm.formState.errors.email.message}
                </p>
              )}
            </div>

            {/* Error message */}
            {forgotPasswordError && (
              <div className="mb-4 p-2 bg-red-100 dark:bg-red-900/30 border border-red-500 text-red-700 dark:text-red-300 rounded-lg text-sm">
                {forgotPasswordError.response?.data?.msg ||
                  'Failed to send verification code. Please try again.'}
              </div>
            )}

            {/* Submit Button */}
            <motion.button
              type="submit"
              disabled={isRequestingPasswordReset}
              className="w-full mt-5 py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-xl transition duration-200"
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              {isRequestingPasswordReset ? 'Sending...' : 'Send Verification Code'}
            </motion.button>
          </form>
        ) : (
          // Reset Password Form
          <form onSubmit={resetPasswordForm.handleSubmit(onResetPassword)} className="mt-6">
            <div className="mb-4">
              <p className="text-gray-600 dark:text-gray-300 text-sm mb-4">
                We&apos;ve sent a verification code to <strong>{passwordResetState.email}</strong>.
                Enter the code and your new password below.
              </p>

              <label className="block text-gray-700 dark:text-gray-300 mb-2">OTP Code</label>
              <input
                type="text"
                className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                  resetPasswordForm.formState.errors.otp ? 'border-red-500' : ''
                }`}
                placeholder="Enter 6-digit code"
                {...resetPasswordForm.register('otp')}
              />
              {resetPasswordForm.formState.errors.otp && (
                <p className="text-red-500 text-sm mt-1">
                  {resetPasswordForm.formState.errors.otp.message}
                </p>
              )}
            </div>

            <div className="mb-4">
              <label className="block text-gray-700 dark:text-gray-300 mb-2">New Password</label>
              <div className="relative">
                <input
                  type={showPassword ? 'text' : 'password'}
                  className={`w-full p-3 pr-10 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                    resetPasswordForm.formState.errors.newPassword ? 'border-red-500' : ''
                  }`}
                  placeholder="Enter new password"
                  {...resetPasswordForm.register('newPassword')}
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute inset-y-0 right-3 flex items-center"
                >
                  {showPassword ? (
                    <EyeSlashIcon className="w-6 h-6 text-gray-600 dark:text-gray-300" />
                  ) : (
                    <EyeIcon className="w-6 h-6 text-gray-600 dark:text-gray-300" />
                  )}
                </button>
              </div>
              {resetPasswordForm.formState.errors.newPassword && (
                <p className="text-red-500 text-sm mt-1">
                  {resetPasswordForm.formState.errors.newPassword.message}
                </p>
              )}
            </div>

            <div className="mb-4">
              <label className="block text-gray-700 dark:text-gray-300 mb-2">
                Confirm Password
              </label>
              <input
                type={showPassword ? 'text' : 'password'}
                className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
                  resetPasswordForm.formState.errors.confirmPassword ? 'border-red-500' : ''
                }`}
                placeholder="Confirm new password"
                {...resetPasswordForm.register('confirmPassword')}
              />
              {resetPasswordForm.formState.errors.confirmPassword && (
                <p className="text-red-500 text-sm mt-1">
                  {resetPasswordForm.formState.errors.confirmPassword.message}
                </p>
              )}
            </div>

            {/* Error message */}
            {recoverPasswordError && (
              <div className="mb-4 p-2 bg-red-100 dark:bg-red-900/30 border border-red-500 text-red-700 dark:text-red-300 rounded-lg text-sm">
                {recoverPasswordError.response?.data?.msg ||
                  'Failed to reset password. Please try again.'}
              </div>
            )}

            {/* Submit Button */}
            <motion.button
              type="submit"
              disabled={isRecoveringPassword}
              className="w-full mt-5 py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-xl transition duration-200"
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              {isRecoveringPassword ? 'Resetting...' : 'Reset Password'}
            </motion.button>
          </form>
        )}

        {/* Back to Login */}
        <div className="mt-6 text-center">
          <Link to="/auth/login">
            <span className="text-blue-500 hover:underline text-sm">Back to Login</span>
          </Link>
        </div>
      </motion.div>
    </div>
  );
}
