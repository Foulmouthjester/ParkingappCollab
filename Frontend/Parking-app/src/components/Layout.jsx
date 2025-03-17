import { NavBar } from "./NavBar"
import { Footer } from "./Footer"

export const Layout = ({children}) => {
  return (
    <div className="layout-container">
        <NavBar />
        <main className="main-container">{children}</main>
        <Footer />
    </div>
  )
}
