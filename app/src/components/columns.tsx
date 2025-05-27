import type { ColumnDef } from "@tanstack/react-table"
import type { Product } from "../api"
import { DataTableColumnHeader } from "./column-header"
 
export const columns: ColumnDef<Product>[] = [
    {
        accessorKey: "category",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Category" />),
        cell: ({ row }) => (<div className="px-4">{row.getValue("category")}</div>)
    },
    {
        accessorKey: "name",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Name" />),
        cell: ({ row }) => (<div className="px-4">{row.getValue("name")}</div>)
    },
    {
        accessorKey: "productCode",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Product code" />),
        cell: ({ row }) => (<div className="px-4">{row.getValue("productCode")}</div>)
    },
    {
        accessorKey: "price",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Price" className="text-right" />),
        cell: ({ row }) => {
            const price = parseFloat(row.getValue("price"))
            const formatted = new Intl.NumberFormat("en-GB", {
                style: "currency",
                currency: "GBP",
            }).format(price)
          return <div className="text-right font-medium px-4">{formatted}</div>
        },
    },
    {
        accessorKey: "quantity",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Quantity" className="text-right" />),
        cell: ({ row }) => (<div className="text-right font-medium px-4">{row.getValue("quantity")}</div>)
    },
    {
        accessorKey: "dateAdded",
        header: ({ column }) => (<DataTableColumnHeader column={column} title="Date added" />),
        cell: ({ row }) => { 
            const date = row.getValue<Date>("dateAdded").toLocaleString();
            return <div className="px-4">{date}</div>     
        }
    }
]