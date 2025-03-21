
import React from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.jsx'; 
import { StrictMode } from 'react';

const root = createRoot(document.getElementById('root')); // Create a root
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);







