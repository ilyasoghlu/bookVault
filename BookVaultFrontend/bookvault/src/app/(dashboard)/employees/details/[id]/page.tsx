"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getEmployeeById } from '@/lib/employeeService';
import { Employee } from '@/types/employee';
import EmployeeDetailsDialog from '@/components/employees/EmployeeDetailsDialog';






function EmployeeDetailsPage() {
  const params = useParams()
    const id = Number(params.id);
    const router = useRouter();
    const [employee, setEmployee] = useState<Employee | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);
  
    useEffect(() =>{
      const fetchEmployee = async() =>{
        try {
          const data = await getEmployeeById(id);
          setEmployee(data)
        } catch (error) {
          console.error("Error", error);
          setError("Employee information can't be found.")
        }finally{
          setLoading(false)
        }
      }
      fetchEmployee();
    }, [id]);
  
    if(loading) {
      return <div>Loading..</div>;
    }
    
    if(error || !employee) {
      return <div>{error ?? "Emploee not found."}</div>
    }
  
  return (
    <>
     <EmployeeDetailsDialog employee={employee} open={true} onOpenChange={(open) => {
       if (!open) {
         router.push('/employees');
       }
     }} />
    </>
  )
}

export default EmployeeDetailsPage