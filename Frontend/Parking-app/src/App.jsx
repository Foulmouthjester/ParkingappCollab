import React, { useState } from 'react';
import './App.css';
import RedirectPage from './RedirectPage'; // Import the redirect page

function App() {
  const [showSignup, setShowSignup] = useState(false);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [skipLogin, setSkipLogin] = useState(false);

  if (skipLogin) {
    return <RedirectPage />;
  }

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    if (id === 'username') setUsername(value);
    else if (id === 'password') setPassword(value);
    else if (id === 'confirm-password') setConfirmPassword(value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (showSignup && password !== confirmPassword) {
      alert("Passwords don't match!");
      return;
    }
    console.log(showSignup ? 'Sign Up' : 'Login', { username, password });
    setUsername('');
    setPassword('');
    setConfirmPassword('');
    setSkipLogin(true);
  };

  const handleSkipLogin = () => {
    setSkipLogin(true);
  };

  const handleToggle = () => {
    setShowSignup(!showSignup);
  };

  return (
    <div className="login-container">
      <h2>{showSignup ? 'Sign Up' : 'Login'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="username">Username</label>
          <input type="text" id="username" value={username} onChange={handleInputChange} placeholder="Enter your username" required />
        </div>
        <div className="input-group">
          <label htmlFor="password">Password</label>
          <input type="password" id="password" value={password} onChange={handleInputChange} placeholder="Enter your password" required />
        </div>
        {showSignup && (
          <div className="input-group">
            <label htmlFor="confirm-password">Confirm Password</label>
            <input type="password" id="confirm-password" value={confirmPassword} onChange={handleInputChange} placeholder="Confirm your password" required />
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

export default App;

