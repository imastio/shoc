import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import Link from "next/link";
import HomeIcon from "@/components/icons/home-icon";
import { Badge } from "@/components/ui/badge";
import MenuToggleButton from "@/components/layout/menu-toggle-button";

export default function WorkspaceMobileSidebar(){

    return <Sheet>
    <SheetTrigger asChild>
      <MenuToggleButton />
    </SheetTrigger>
    <SheetContent side="left" className="flex flex-col">
      <nav className="grid gap-2 text-lg font-medium">
        <Link href="#" className="flex items-center gap-2 text-lg font-semibold" prefetch={false}>
          <span className="sr-only">Acme Inc</span>
        </Link>
        <Link
          href="#"
          className="mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground"
          prefetch={false}
        >
          <HomeIcon className="h-5 w-5" />
          Dashboard
        </Link>
        <Link
          href="#"
          className="mx-[-0.65rem] flex items-center gap-4 rounded-xl bg-muted px-3 py-2 text-foreground hover:text-foreground"
          prefetch={false}
        >
          Orders
          <Badge className="ml-auto flex h-6 w-6 shrink-0 items-center justify-center rounded-full">6</Badge>
        </Link>
        <Link
          href="#"
          className="mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground"
          prefetch={false}
        >
          Products
        </Link>
        <Link
          href="#"
          className="mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground"
          prefetch={false}
        >
          Customers
        </Link>
        <Link
          href="#"
          className="mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground"
          prefetch={false}
        >
          Analytics
        </Link>
      </nav>
      <div className="mt-auto">
        <Card>
          <CardHeader>
            <CardTitle>Upgrade to Pro</CardTitle>
            <CardDescription>Unlock all features and get unlimited access to our support team.</CardDescription>
          </CardHeader>
          <CardContent>
            <Button size="sm" className="w-full">
              Upgrade
            </Button>
          </CardContent>
        </Card>
      </div>
    </SheetContent>
  </Sheet>

}