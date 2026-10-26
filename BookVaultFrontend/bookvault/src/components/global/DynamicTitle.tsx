'use client'

import React from 'react'
import { usePathname } from 'next/navigation';
import PageTitle from './PageTitle';
import Dashboard from '../dashboard/Dashboard';


function DynamicTitle() {
  const pathname = usePathname()

    // const isProductPage = pathname.startsWith('/portfolio/')&& pathname.split('/').length ===3
    const titles: Record<string, string> = {
        "/dashboard": "Dashboard",
        "/authors": "Authors",
        "/bookcases": "Book Cases",
        "/books": "Books",
        "/borrowings": "Borrowings",
        "/employees": "Employees",
        "/publishers": "Publishers",
        "/readers": "Readers",
        "/rooms": "Rooms",
        "/sections": "Sections",
        "/shelves": "Shelves"

        };

    // ✅ Handle dynamic category route
    if (pathname.startsWith('/details/')) {
        const details = pathname.split('/')[2] // get dynamic part

        return <PageTitle text={`${details} `}  />
    }

    const text = titles[pathname] || 'Page';
    if(pathname === '/'){
        return<Dashboard />
    }
    // if (isProductPage) {
    //     return <ProductBreadCrumbs productId={productId} />
    // }
    return<PageTitle text={text} />
}

export default DynamicTitle
