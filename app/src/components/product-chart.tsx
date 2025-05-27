import { Bar, BarChart, CartesianGrid, XAxis } from "recharts"
import { type ChartConfig, ChartContainer, ChartLegend, ChartLegendContent } from "@/components/ui/chart"
 
const chartConfig = {
  quantity: {
    label: "Quantity",
    color: "#2563eb",
  },
} satisfies ChartConfig

export type ProductData = {
    category: string,
    quantity: number
}

interface ProductChartProps {
  data: ProductData[]
}

export function ProductChart({ data } : ProductChartProps) {
return (
    <ChartContainer config={chartConfig} className="min-h-[200px] w-72 h-72">
      <BarChart accessibilityLayer data={data}>
        <CartesianGrid vertical={false} />
        <Bar dataKey="quantity" fill="var(--color-quantity)" radius={4} />
        <XAxis
            dataKey="category"
            tickLine={false}
            tickMargin={10}
            axisLine={false}
        />
        <ChartLegend content={<ChartLegendContent />} />
      </BarChart>
    </ChartContainer>
  )
}