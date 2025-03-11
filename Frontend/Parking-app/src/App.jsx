import React from 'react';
import LoginScreen from './pages/LoginScreen';
import { BrowserRouter, Routes, Route } from 'react-router';
import { Contact } from './pages/Contact';
import { About } from './pages/About';


const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginScreen />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;


