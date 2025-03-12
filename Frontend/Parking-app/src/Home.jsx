import { CallToAction } from "./CallToAction";
import { Footer } from "./Footer";
import { MainContent } from "./MainContent";
import { NavBar } from "./NavBar";

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