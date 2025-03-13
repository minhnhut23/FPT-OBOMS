import { useState } from 'react';
import { motion } from 'framer-motion';
import { EyeIcon, EyeSlashIcon, ArrowLeftIcon } from '@heroicons/react/24/outline'; // Import icons
import { Link } from 'react-router-dom';

export default function Login() {
  const [showPassword, setShowPassword] = useState(false);

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

      {/* Login Form */}
      <motion.div
        className="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg relative z-10"
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="flex items-center justify-between mb-4">
          <Link to="/">
            <motion.button
              onClick={() => setExitAnimation(true)}
              className="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition"
              whileHover={{ scale: 1.1 }} // Scale up slightly on hover
              whileTap={{ scale: 0.9 }} // Optional: Shrink slightly when clicked
            >
              <ArrowLeftIcon className="w-6 h-6 cursor-pointer" />
            </motion.button>
          </Link>

          <div className="text-3xl font-bold text-center flex-1">Login</div>

          {/* Empty span to balance layout */}
          <span className="w-6"></span>
        </div>

        <div className="mt-6">
          {/* Email Input */}
          <label className="block text-gray-700 dark:text-gray-300 mb-2">Email</label>
          <input
            type="email"
            className="w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 mb-5" // Increased margin-bottom
            placeholder="Enter your email"
          />

          {/* Password Input with Eye Icon Inside */}
          <label className="block text-gray-700 dark:text-gray-300 mb-2">Password</label>
          <div className="relative w-full">
            <input
              type={showPassword ? 'text' : 'password'}
              className="w-full p-3 pr-10 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
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

          {/* Remember Me & Forgot Password Section */}
          <div className="mt-5 flex justify-between items-center">
            {/* Remember Me Checkbox */}
            <label className="flex items-center text-gray-600 dark:text-gray-300 text-sm">
              <input
                type="checkbox"
                className="w-3 h-3 text-blue-500 bg-gray-200 border-gray-300 rounded focus:ring focus:ring-blue-500"
              />
              <span className="ml-2">Remember me</span>
            </label>

            {/* Forgot Password Link */}
            <a href="#" className="text-blue-500 hover:underline text-sm">
              Forgot password?
            </a>
          </div>
        </div>

        {/* Login Button */}
        <motion.button
          onClick={() => setExitAnimation(true)}
          className="w-full mt-5 py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-xl transition duration-200"
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
        >
          Login
        </motion.button>

        {/* Sign Up Section */}
        <Link to="/auth/register">
          <div className="mt-4 text-center">
            <span className="text-gray-600 dark:text-gray-300 text-sm">Don't have an account?</span>
            <a href="#" className="text-blue-500 hover:underline text-sm font-medium ms-1">
              Create yours!
            </a>
          </div>
        </Link>
      </motion.div>
    </div>
  );
}
