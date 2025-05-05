import MainNav from "./main-nav"
import AuthSwitch from "./auth-switch"
import MobileNav from "./mobile-nav"

export function SiteHeader() {
  return (
    <header className="sticky top-0 z-50 w-full border-b border-border/40 bg-background/95 backdrop-blur-sm supports-backdrop-filter:bg-background/60">
      <div className="md:mx-auto mx-[8px] md:container overflow-x flex h-14 max-w-(--breakpoint-2xl) items-center">
        <MainNav />
        <div className="flex flex-1 items-center justify-between space-x-2 md:justify-end">
          <div className="hidden sm:flex w-full flex-1 md:w-auto md:flex-none">
          </div>
          <nav className="flex items-center">
            <AuthSwitch />
            <MobileNav />
          </nav>
        </div>
      </div>
    </header>
  )
}