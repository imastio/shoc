"use client"

import useRouteAccess from "@/access/use-route-access";
import { usePathname, useRouter } from "next/navigation";
import { ReactNode, useEffect, useState } from "react"

export default function AccessGuardLayout({ children }: { children: ReactNode }) {

   const pathname = usePathname();
   const { isAllowed } = useRouteAccess();
   const router = useRouter();

   const [authorized, setAuthorized] = useState<boolean | null>(null);

   useEffect(() => {
      if (!pathname) {
         return;
      }

      if (!isAllowed(pathname)) {
         router.replace("/access-denied");
         setAuthorized(false);
      } else {
         setAuthorized(true);
      }
   }, [pathname, isAllowed, router]);

   if (authorized === null) {
      return false;
   }

   if (!authorized) {
      return false;
   }

   return children;
}
