import { NavBar } from "./NavBar"
import { Footer } from "./Footer"


export const Layout = ({children}) => {
  return (
    <div>
        <NavBar />
        <main>{children}</main>
        <Footer />
    </div>
  )
}
