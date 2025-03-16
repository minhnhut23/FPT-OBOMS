import { useState } from 'react';
import { motion } from 'framer-motion';
import useAuth from '../../hooks/useAuth';

export default function Dashboard() {
  const { user, logout } = useAuth();
  const [isLoading, setIsLoading] = useState(false);

  const handleLogout = async () => {
    setIsLoading(true);
    await logout();
    setIsLoading(false);
  };

  return (
    <div className="min-h-screen bg-background-primary text-text-primary">
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-3xl font-bold">Dashboard</h1>
          <motion.button
            onClick={handleLogout}
            disabled={isLoading}
            className="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-colors"
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            {isLoading ? 'Logging out...' : 'Logout'}
          </motion.button>
        </div>

        <div className="bg-background-secondary p-6 rounded-xl shadow-md">
          <h2 className="text-xl font-semibold mb-4">
            Welcome, {user?.fullName || user?.email || 'User'}
          </h2>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mt-6">
            <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow-sm">
              <h3 className="text-lg font-medium mb-2">Profile Information</h3>
              <div className="space-y-2">
                <p className="text-gray-700 dark:text-gray-300">
                  <span className="font-medium">Email:</span> {user?.email || 'N/A'}
                </p>
                <p className="text-gray-700 dark:text-gray-300">
                  <span className="font-medium">Full Name:</span> {user?.fullName || 'N/A'}
                </p>
                {user?.bio && (
                  <p className="text-gray-700 dark:text-gray-300">
                    <span className="font-medium">Bio:</span> {user?.bio}
                  </p>
                )}
                <p className="text-gray-700 dark:text-gray-300">
                  <span className="font-medium">Role:</span>{' '}
                  {user?.role === 0 ? 'Customer' : user?.role === 1 ? 'Admin' : 'Unknown'}
                </p>
              </div>
            </div>

            <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow-sm">
              <h3 className="text-lg font-medium mb-2">Account Settings</h3>
              <div className="space-y-3">
                <button className="w-full text-left px-4 py-2 bg-blue-50 dark:bg-blue-900/30 text-blue-600 dark:text-blue-300 rounded-md hover:bg-blue-100 dark:hover:bg-blue-900/50 transition-colors">
                  Change Password
                </button>
                <button className="w-full text-left px-4 py-2 bg-blue-50 dark:bg-blue-900/30 text-blue-600 dark:text-blue-300 rounded-md hover:bg-blue-100 dark:hover:bg-blue-900/50 transition-colors">
                  Update Profile Information
                </button>
                <button className="w-full text-left px-4 py-2 bg-blue-50 dark:bg-blue-900/30 text-blue-600 dark:text-blue-300 rounded-md hover:bg-blue-100 dark:hover:bg-blue-900/50 transition-colors">
                  Notification Settings
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
