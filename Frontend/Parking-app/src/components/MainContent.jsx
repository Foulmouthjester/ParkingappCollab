import { useNavigate } from 'react-router-dom';
export const MainContent = () => {
    const navigate = useNavigate();
    return (
        <div className="main-content">
            <div className="sharp-text">
                    <h1>Welcome to EcoPark</h1>
                    <h2>Park with purpose</h2>
           
                 <div className="button-selection">
                    <div><button onClick={() => navigate('/register')}>Register</button></div>
                    <div><button onClick={() => navigate('/login')}>Log In</button></div>
                </div>
            
            </div>
        </div>
    );
}