import { useState } from 'react';
import { motion } from 'framer-motion';
import { EyeIcon, EyeSlashIcon } from '@heroicons/react/24/outline'; // Import icons

export default function Login() {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className="min-h-screen bg-white dark:bg-gray-900 text-gray-900 dark:text-white flex items-center justify-center px-4">
      {/* Background Effects */}
      <div className="absolute inset-0 w-full h-full">
        <div className="absolute top-1/4 -left-1/4 w-72 sm:w-96 h-72 sm:h-96 bg-purple-500/30 dark:bg-purple-500/30 rounded-full filter blur-3xl" />
        <div className="absolute -top-24 -right-1/4 w-72 sm:w-96 h-72 sm:h-96 bg-blue-500/30 dark:bg-blue-500/30 rounded-full filter blur-3xl" />
      </div>

      {/* Login Form */}
      <motion.div
        className="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-lg shadow-lg relative z-10"
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="text-3xl font-bold text-center">Login</div>

        <div className="mt-4">
          {/* Email Input */}
          <label className="block text-gray-700 dark:text-gray-300">Email</label>
          <input
            type="email"
            className="w-full mt-2 mb-4 p-3 border rounded-lg bg-gray-200 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your email"
          />

          {/* Password Input with Eye Icon Inside */}
          <label className="block text-gray-700 dark:text-gray-300">Password</label>
          <div className="relative w-full">
            <input
              type={showPassword ? 'text' : 'password'}
              className="w-full p-3 pr-10 border rounded-lg bg-gray-200 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter your password"
            />

            {/* Show/Hide Password Button */}
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

          {/* Forgot Password Link */}
          <div className="mt-2 text-right">
            <a href="#" className="text-blue-500 hover:underline text-sm">
              Forgot password?
            </a>
          </div>
        </div>

        {/* Login Button */}
        <button className="w-full mt-4 py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-lg transition duration-200">
          Login
        </button>

        {/* Sign Up Section */}
        <div className="mt-4 text-center">
          <span className="text-gray-600 dark:text-gray-300 text-sm">Don't have an account?</span>
          <a href="#" className="text-blue-500 hover:underline text-sm font-medium ms-1">
            Sign up
          </a>
        </div>
      </motion.div>
    </div>
  );
}
