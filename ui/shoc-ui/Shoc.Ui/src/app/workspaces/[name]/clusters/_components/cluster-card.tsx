import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { MoreVertical, Pencil, Trash2 } from "lucide-react"
import ClusterIcon from "@/components/icons/cluster-icon";

interface ObjectCardProps {
  title: string;
  status: 'active' | 'archived';
}

export default function Component({ title, status }: ObjectCardProps = { title: 'Sample Object', status: 'active' }) {
  return (
    <Card className="w-full max-w-sm">
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <div className="flex items-center space-x-2">
          <ClusterIcon className="h-6 w-6 text-primary" />
          <h2 className="text-lg font-semibold">{title}</h2>
        </div>
        <Badge variant={status === 'active' ? 'default' : 'secondary'}>
          {status}
        </Badge>
      </CardHeader>
      <CardContent>
        <p className="text-sm text-muted-foreground">
          This is a brief description or additional information about the object.
        </p>
      </CardContent>
      <CardFooter className="flex justify-between">
        <div className="flex space-x-2">
          <Button variant="outline" size="sm">
            <Pencil className="h-4 w-4 mr-2" />
            Edit
          </Button>
          <Button variant="outline" size="sm">
            <Trash2 className="h-4 w-4 mr-2" />
            Delete
          </Button>
        </div>
        <Button variant="ghost" size="sm">
          <MoreVertical className="h-4 w-4" />
          <span className="sr-only">More options</span>
        </Button>
      </CardFooter>
    </Card>
  )
}