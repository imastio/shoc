"use client"

import useRouteAccess from "@/access/use-route-access";
import { usePathname, useRouter } from "next/navigation";
import { ReactNode } from "react"

export default function AccessGuardLayout({ children } : { children: ReactNode }) {

   const pathname = usePathname();
   const { isAllowed } = useRouteAccess();
   const router = useRouter();

   if(!pathname){
      return false;
   }

   if(!isAllowed(pathname)){
      console.log(`No access to ${pathname}, forbidden.`)
      router.replace('/access-denied');
      return false;
   }

   return children;
}
