import { useState } from 'react';
import { motion } from 'framer-motion';
import { EyeIcon, EyeSlashIcon, ArrowLeftIcon } from '@heroicons/react/24/outline'; // Import icons
import { Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import useAuth from '../../hooks/useAuth';

// Validation schema
const registerSchema = yup.object({
  fullName: yup.string().required('Full name is required'),
  birthDate: yup.date().nullable(),
  email: yup.string().email('Please enter a valid email').required('Email is required'),
  password: yup
    .string()
    .required('Password is required')
    .min(8, 'Password must be at least 8 characters')
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/,
      'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character'
    ),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Passwords must match')
    .required('Confirm password is required'),
});

export default function Register() {
  const [showPassword, setShowPassword] = useState(false);
  const { register: registerUser, isRegistering, registerError } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(registerSchema),
    defaultValues: {
      fullName: '',
      birthDate: '',
      email: '',
      password: '',
      confirmPassword: '',
    },
  });

  const onSubmit = (data) => {
    const { confirmPassword, ...userData } = data;
    console.log(confirmPassword);
    registerUser(userData);
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

      {/* Register Form */}
      <motion.div
        className="w-full max-w-md bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg relative z-10"
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        {/* Title with Rollback Icon */}
        <div className="flex items-center justify-between mb-4">
          <Link to="/">
            <motion.button
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
        <form onSubmit={handleSubmit(onSubmit)} className="mt-6">
          <div className="mb-4">
            <input
              type="text"
              className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.fullName ? 'border-red-500' : ''}`}
              placeholder="Enter your full name"
              {...register('fullName')}
            />
            {errors.fullName && (
              <p className="text-red-500 text-sm mt-1">{errors.fullName.message}</p>
            )}
          </div>

          <div className="mb-4">
            <input
              type="date"
              className={`w-full p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-gray-400 bg-gray-100 dark:bg-gray-700 dark:[color-scheme:dark] custom-datepicker ${errors.birthDate ? 'border-red-500' : ''}`}
              {...register('birthDate')}
            />
            {errors.birthDate && (
              <p className="text-red-500 text-sm mt-1">{errors.birthDate.message}</p>
            )}
          </div>

          <div className="mb-4">
            <input
              type="email"
              className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.email ? 'border-red-500' : ''}`}
              placeholder="Enter your email"
              {...register('email')}
            />
            {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>}
          </div>

          <div className="relative w-full mb-4">
            <input
              type={showPassword ? 'text' : 'password'}
              className={`w-full p-3 pr-10 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.password ? 'border-red-500' : ''}`}
              placeholder="Enter your password"
              {...register('password')}
            />
            {/* Show/Hide Password Button */}
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute inset-y-0 right-3 flex items-center"
            >
              {showPassword ? (
                <EyeSlashIcon className="w-6 h-6 text-gray-400 dark:text-gray-400" />
              ) : (
                <EyeIcon className="w-6 h-6 text-gray-400 dark:text-gray-400" />
              )}
            </button>
            {errors.password && (
              <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>
            )}
          </div>

          <div className="mb-4">
            <input
              type={showPassword ? 'text' : 'password'}
              className={`w-full p-3 border rounded-lg bg-gray-100 dark:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${errors.confirmPassword ? 'border-red-500' : ''}`}
              placeholder="Confirm your password"
              {...register('confirmPassword')}
            />
            {errors.confirmPassword && (
              <p className="text-red-500 text-sm mt-1">{errors.confirmPassword.message}</p>
            )}
          </div>

          {/* Registration error message */}
          {registerError && (
            <div className="mb-4 p-2 bg-red-100 dark:bg-red-900/30 border border-red-500 text-red-700 dark:text-red-300 rounded-lg text-sm">
              {registerError.response?.data?.msg || 'Registration failed. Please try again.'}
            </div>
          )}

          {/* Register Button */}
          <motion.button
            type="submit"
            disabled={isRegistering}
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
            className="w-full py-3 bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-xl transition duration-200"
          >
            {isRegistering ? 'Registering...' : 'Register'}
          </motion.button>
        </form>
        <div className="mt-4 text-center">
          <span className="text-gray-600 dark:text-gray-300 text-sm">Already have an account?</span>
          <Link to="/auth/login">
            <span className="text-blue-500 hover:underline text-sm font-medium ms-1">Log in</span>
          </Link>
        </div>
      </motion.div>
    </div>
  );
}
