'use client'

import * as React from "react"
import { cn } from "@/lib/utils"
import { usePathname } from "next/navigation"
import Link from "next/link"

export default function MainNav() {
  const pathname = usePathname();
  
  const mainNav: any[] = [
  ]

  return (
    <div className="mr-4 flex  w-full">
      <Link href="/" className="mr-6 flex items-center space-x-2 h-[40px]">
        <h1>Shoc Platform</h1>
      </Link>
      <nav className="flex items-center space-x-6 text-sm font-medium hidden md:flex">
        {
          mainNav.map((nav, index) => <Link key={index}
          href={nav.href}
          className={cn(
            "transition-colors hover:text-foreground/80",
            pathname?.indexOf(nav.href) !== -1 ? "text-foreground" : "text-foreground/60"
          )}
        >
          {nav.title}
        </Link>)
        }
      </nav>
    </div>
  )
}