// src/App.js
import React, { useState } from 'react';
import './App.css';

function App() {
  // State to toggle between login and signup form
  const [showSignup, setShowSignup] = useState(false);

  // State for form fields (username, password, and confirm password)
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  // Handle form input changes
  const handleInputChange = (e) => {
    const { id, value } = e.target;
    if (id === 'username') {
      setUsername(value);
    } else if (id === 'password') {
      setPassword(value);
    } else if (id === 'confirm-password') {
      setConfirmPassword(value);
    }
  };

  // Handle form submission
  const handleSubmit = (e) => {
    e.preventDefault(); // Prevent default form submission

    // If we're in the signup form, ensure passwords match
    if (showSignup && password !== confirmPassword) {
      alert("Passwords don't match!");
      return;
    }

    // Handle Login or Sign Up logic here
    if (showSignup) {
      console.log('Sign Up:', { username, password });
      // Perform Sign Up
    } else {
      console.log('Login:', { username, password });
      // Perform Login
    }

    // Optionally clear the form fields after submission
    setUsername('');
    setPassword('');
    setConfirmPassword('');
  };

  // Toggle between Login and Sign Up forms
  const handleToggle = () => {
    setShowSignup(!showSignup); // Toggle state to show the other form
  };

  return (
    <div className="login-container">
      <h2>{showSignup ? 'Sign Up' : 'Login'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="username">{showSignup ? 'Username' : 'Username'}</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={handleInputChange}
            placeholder="Enter your username"
            required
          />
        </div>
        <div className="input-group">
          <label htmlFor="password">{showSignup ? 'Password' : 'Password'}</label>
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
        <span onClick={handleToggle} className="toggle-link">
          {showSignup ? 'Login' : 'Sign Up'}
        </span>
      </p>
    </div>
  );
}

export default App;
