import Link from "next/link";
import { buttonVariants } from "@/components/ui/button";
import PublicLogo from "./public-logo";
import { PublicNavMenu } from "./public-nav-menu";
import { GitHubLogoIcon, TwitterLogoIcon } from "@radix-ui/react-icons";
import staticLinks from "@/well-known/static-links";

export function PublicNavbar() {
  return (
    <nav className="w-full border-b h-16 sticky top-0 z-50 bg-background">
      <div className="sm:container mx-auto w-[95vw] h-full flex items-center justify-between md:gap-2">
        <div className="flex items-center gap-5">
          <div className="flex items-center gap-6">
            <div className="flex">
              <PublicLogo />
            </div>
            <div className="lg:flex hidden items-center gap-4 text-sm font-medium text-muted-foreground">
              <PublicNavMenu />
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <div className="flex items-center gap-2">
            <div className="flex ml-2.5 sm:ml-0">
              <Link
                href={staticLinks.githubRepo}
                className={buttonVariants({ variant: "ghost", size: "icon" })}
              >
                <GitHubLogoIcon className="h-[1.1rem] w-[1.1rem]" />
              </Link>
              <Link
                href="#"
                className={buttonVariants({
                  variant: "ghost",
                  size: "icon",
                })}
              >
                <TwitterLogoIcon className="h-[1.1rem] w-[1.1rem]" />
              </Link>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
}
