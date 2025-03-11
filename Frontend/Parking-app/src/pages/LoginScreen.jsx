import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import './LoginScreen.css';

const LoginScreen = () => {
  const { register, handleSubmit, watch, formState: { errors } } = useForm();
  const navigate = useNavigate();
  const [isRegistering, setIsRegistering] = useState(false);

  const handleLogin = (data) => {
    console.log('Logging in with:', data);
  };

  const handleRegister = (data) => {
    console.log('Registering with:', data);
    navigate('/register');
  };

  return (
    <div className="container">
      <h2>{isRegistering ? 'Create Account' : 'Sign In'}</h2>

      <form onSubmit={handleSubmit(isRegistering ? handleRegister : handleLogin)}>
        <div className="form-group">
          <input
            type="email"
            placeholder="Email"
            {...register('email', { required: 'Email is mandatory', pattern: { value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i, message: 'Invalid Email' } })}
            className="input"
          />
          {errors.email && <p className="error">{errors.email.message}</p>}
        </div>

        <div className="form-group">
          <input
            type="password"
            placeholder="Password"
            {...register('password', { required: 'Password is mandatory', minLength: { value: 6, message: 'Password must be at least 6 characters' } })}
            className="input"
          />
          {errors.password && <p className="error">{errors.password.message}</p>}
        </div>

        {isRegistering && (
          <div className="form-group">
            <input
              type="password"
              placeholder="Confirm Password"
              {...register('confirmPassword', {
                required: 'Confirm Password is required',
                validate: (value) => value === watch('password') || 'Passwords do not match',
              })}
              className="input"
            />
            {errors.confirmPassword && <p className="error">{errors.confirmPassword.message}</p>}
          </div>
        )}

        <button type="submit">{isRegistering ? 'Create Account' : 'Sign in'}</button>
      </form>

      <div className="register-link">
        <span>
          {isRegistering ? (
            <>
              Already have an account?{' '}
              <a href="#" onClick={() => setIsRegistering(false)}>Sign in here</a>
            </>
          ) : (
            <>
              Have no account?{' '}
              <a href="#" onClick={() => setIsRegistering(true)}>Register here</a>
            </>
          )}
        </span>
      </div>
    </div>
  );
};

export default LoginScreen;

