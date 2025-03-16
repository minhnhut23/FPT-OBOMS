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

const LandingPage = lazy(() => import('./pages/LandingPage'));
const Login = lazy(() => import('./pages/auth/Login'));
const Register = lazy(() => import('./pages/auth/Register'));
const ForgotPassword = lazy(() => import('./pages/auth/ForgotPassword'));

export default function App() {
  return (
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider>
          <AuthProvider>
            <Routes>
              <Route
                path="/"
                element={
                  <Suspense fallback={<LoadingSpinner />}>
                    <LandingPage />
                  </Suspense>
                }
              />

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
            </Routes>
          </AuthProvider>
        </ThemeProvider>
        <ReactQueryDevtools initialIsOpen={false} />
        <Toaster position="top-right" />
      </QueryClientProvider>
    </Provider>
  );
}
