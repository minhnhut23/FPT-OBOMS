import { Provider } from 'react-redux';
import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Routes, Route } from 'react-router-dom';
import { store } from './store';
import { queryClient } from './lib/react-query';
import { ThemeProvider } from './context/ThemeContext';
import { lazy, Suspense } from 'react';
import { LoadingSpinner } from './components/LoadingSpinner';
import { Toaster } from 'react-hot-toast';
import AuthProvider from './components/AuthProvider';
import { PublicRoute } from './routes/PublicRoute';
import { ProtectedRoute } from './routes/ProtectedRoute';

// Public pages
const LandingPage = lazy(() => import('./pages/LandingPage'));
const Login = lazy(() => import('./pages/auth/Login'));
const Register = lazy(() => import('./pages/auth/Register'));
const ForgotPassword = lazy(() => import('./pages/auth/ForgotPassword'));

// Protected pages (will be used when user is authenticated)
const Dashboard = lazy(() => import('./pages/app/Dashboard'));

export default function App() {
  return (
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider>
          <AuthProvider>
            <Routes>
              {/* Public Routes - No Authentication Required */}
              <Route element={<PublicRoute />}>
                {/* Landing Page */}
                <Route
                  path="/"
                  element={
                    <Suspense fallback={<LoadingSpinner />}>
                      <LandingPage />
                    </Suspense>
                  }
                />

                {/* Authentication Routes */}
                <Route
                  path="auth/login"
                  element={
                    <Suspense fallback={<LoadingSpinner />}>
                      <Login />
                    </Suspense>
                  }
                />

                <Route
                  path="auth/register"
                  element={
                    <Suspense fallback={<LoadingSpinner />}>
                      <Register />
                    </Suspense>
                  }
                />

                <Route
                  path="auth/forgot-password"
                  element={
                    <Suspense fallback={<LoadingSpinner />}>
                      <ForgotPassword />
                    </Suspense>
                  }
                />
              </Route>

              {/* Protected Routes - Authentication Required */}
              <Route element={<ProtectedRoute />}>
                <Route
                  path="/app"
                  element={
                    <Suspense fallback={<LoadingSpinner />}>
                      <Dashboard />
                    </Suspense>
                  }
                />

                {/* Add more protected routes here as needed */}
              </Route>

              {/* Default Fallback for Unknown Routes */}
              <Route
                path="*"
                element={
                  <Suspense fallback={<LoadingSpinner />}>
                    <LandingPage />
                  </Suspense>
                }
              />
            </Routes>
          </AuthProvider>
        </ThemeProvider>
        <ReactQueryDevtools initialIsOpen={false} />
        <Toaster position="top-right" />
      </QueryClientProvider>
    </Provider>
  );
}
