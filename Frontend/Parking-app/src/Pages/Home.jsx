import { CallToAction } from "../Components/CallToAction";
import { Footer } from "../Components/Footer";
import { MainContent } from "../Components/MainContent";
import { NavBar } from "../Components/NavBar";

export const Home = () => {
    return (
        <div>
            <NavBar />
            <MainContent />
            <CallToAction />
            <Footer />
        </div>
    ); 
}