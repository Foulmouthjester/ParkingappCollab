import { useNavigate } from 'react-router-dom';

export const NavBar = () => {
    const navigate = useNavigate();
    
    return (
        <div className="header">
            <div className="navbar">
                <div className="title-container">
                    <h1 className="title" onClick={() => navigate('/')}>EcoPark</h1>
                </div>
                <img src="/src/assets/ecopark.svg" alt="EcoPark Logo" className="logo" onClick={() => navigate('/')}/>
                <h2 className="tagline">Park with purpose</h2>
            </div>
        </div>
    );
};