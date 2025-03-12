import { useNavigate } from 'react-router-dom';
export const NavBar = () => {
    const navigate = useNavigate();
    return (
        <div className="header">
            <div className="navbar">
                <h1 className="title">EcoPark</h1>
                <img src="/src/assets/ecopark.svg" alt="EcoPark Logo" className="logo" onClick={() => navigate('/')}/>
                <h2 className="title">Park with purpose</h2>
            </div>
        </div>
    );
};