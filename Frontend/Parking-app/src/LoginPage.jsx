import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './App.css'; // Reuse the same styles

function LoginPage() {
  const [showSignup, setShowSignup] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    if (id === 'email') setEmail(value);
    else if (id === 'password') setPassword(value);
    else if (id === 'confirm-password') setConfirmPassword(value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (showSignup && password !== confirmPassword) {
      setError("Passwords don't match!");
      return;
    }

    const url = showSignup
      ? 'http://localhost:5110/api/parking/register'
      : 'http://localhost:5110/api/parking/login';

    try {
      console.log(showSignup ? 'Registering:' : 'Logging in:', { email, password });
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      const text = await response.text();
      let result;
      try {
        result = JSON.parse(text);
      } catch {
        result = { message: text };
      }
      

      if (response.ok) {        
        localStorage.setItem('userId', result.userId); 
        setEmail('');
        setPassword('');
        setConfirmPassword('');
        navigate(`/redirect?userId=${result.userId}`); 
      } else {
        setError(result.message || `${showSignup ? 'Registration' : 'Login'} failed`);
      }
    } catch (err) {
      setError('Something went wrong. Please try again.');
      console.error('Fetch error:', err);
    }
  };

  const handleSkipLogin = () => {
    localStorage.setItem('userId', '1');
    navigate('/redirect?userId=1');
  };

  const handleToggle = () => {
    setShowSignup(!showSignup);
  };

  return (
    <div className="login-container">
      <h2>{showSignup ? 'Sign Up' : 'Login'}</h2>
      {error && <p className="error">{error}</p>}
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={handleInputChange}
            placeholder="Enter your email"
            required
          />
        </div>
        <div className="input-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={handleInputChange}
            placeholder="Enter your password"
            required
          />
        </div>
        {showSignup && (
          <div className="input-group">
            <label htmlFor="confirm-password">Confirm Password</label>
            <input
              type="password"
              id="confirm-password"
              value={confirmPassword}
              onChange={handleInputChange}
              placeholder="Confirm your password"
              required
            />
          </div>
        )}
        <button type="submit">{showSignup ? 'Sign Up' : 'Login'}</button>
      </form>
      <p className="toggle-text">
        {showSignup ? 'Already have an account? ' : "Don't have an account? "}
        <span onClick={handleToggle} className="toggle-link">{showSignup ? 'Login' : 'Sign Up'}</span>
      </p>
      <button onClick={handleSkipLogin} className="skip-button">Skip Login</button>
    </div>
  );
}

export default LoginPage;