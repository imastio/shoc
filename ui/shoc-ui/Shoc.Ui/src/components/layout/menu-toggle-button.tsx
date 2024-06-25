import MenuIcon from "@/components/icons/menu-icon";
import { Button } from "@/components/ui/button";

export default function MenuToggleButton() {
    return <Button variant="outline" size="icon" className="shrink-0 md:hidden">
        <MenuIcon className="h-5 w-5" />
        <span className="sr-only">Toggle navigation menu</span>
    </Button>
}