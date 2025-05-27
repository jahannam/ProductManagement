import { useQuery } from '@tanstack/react-query'
import { Client } from './api'
import { columns } from "./components/columns"
import { DataTable } from "./components/data-table"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { ProductChart, type ProductData } from './components/product-chart'

function App() {
  const client = new Client("/api");
  const { isPending, error, data } = useQuery({
    queryKey: ['repoData'],
    queryFn: () => client.productAll()
  })
  
  if (isPending) return 'Loading...'

  if (error) return 'An error has occurred: ' + error.message

  const quantityByCategory = data.reduce((acc: Record<string, number>, product) => {
    acc[product.category] = (acc[product.category] || 0) + product.quantity;
    return acc;
  }, {});

  const result: ProductData[] = Object.entries(quantityByCategory).map(([category, quantity]) => ({
    category,
    quantity,
  }));

  return (
    <div className="container px-10 py-5 w-full h-full">
      <header className="text-2xl mb-5 pl-2">
        Product Management
      </header>
      <Tabs defaultValue="table">
        <TabsList>
          <TabsTrigger value="table">Table</TabsTrigger>
          <TabsTrigger value="graph">Graph</TabsTrigger>
        </TabsList>
        <TabsContent value="table"><DataTable columns={columns} data={data} /></TabsContent>
        <TabsContent value="graph"><ProductChart data={result} /></TabsContent>
      </Tabs>
    </div>
  )
}

export default App;
