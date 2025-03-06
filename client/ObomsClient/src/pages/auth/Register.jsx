import { useState } from 'react';
import { motion } from 'framer-motion';
import { EyeIcon, EyeSlashIcon, ArrowLeftIcon, CalendarIcon } from '@heroicons/react/24/outline'; // Import icons
import { Link } from 'react-router-dom';

export default function Register() {
  const [showPassword, setShowPassword] = useState(false);
  const [exitAnimation, setExitAnimation] = useState(false);

  return (
    <div className="min-h-screen bg-gray-100 dark:bg-gray-900 text-gray-900 dark:text-white flex items-center justify-center px-4">
      {/* Background Effects */}
      <div className="absolute inset-0 w-full h-full">
        <div
          className="absolute top-1/4 -left-[20%] w-72 sm:w-96 h-72 sm:h-96 
                  bg-purple-500/30 dark:bg-purple-500/30 
                  rounded-full filter blur-3xl"
        />
        <div
          className="absolute -top-[10%] -right-[20%] w-72 sm:w-96 h-72 sm:h-96 
                  bg-blue-500/30 dark:bg-blue-500/30 
                  rounded-full filter blur-3xl"
        />
      </div>

      {/* Register Form */}
      <motion.div
        className="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-lg shadow-lg relative z-10"
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        {/* Title with Rollback Icon */}
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
          <div className="text-3xl font-bold text-center flex-1">Register</div>
          {/* Empty span to balance layout */}
          <span className="w-6"></span>
        </div>

        <div className="mt-4">
          <input
            type="text"
            className="w-full mb-4 p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your full name"
          />

          <input
            type="date"
            className="w-full mb-4 p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-400  dark:[color-scheme:dark]"
          />

          <input
            type="email"
            className="w-full mb-4 p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your email"
          />

          <div className="relative w-full mb-4">
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

          <input
            type={showPassword ? 'text' : 'password'}
            className="w-full mb-4 p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Confirm your password"
          />
        </div>

        <button className="w-full mt-4 py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-lg transition duration-200">
          Register
        </button>

        <div className="mt-4 text-center">
          <span className="text-gray-600 dark:text-gray-300 text-sm">Already have an account?</span>
          <Link to="/login">
            <span className="text-blue-500 hover:underline text-sm font-medium ms-1">Sign in</span>
          </Link>
        </div>
      </motion.div>
    </div>
  );
}
