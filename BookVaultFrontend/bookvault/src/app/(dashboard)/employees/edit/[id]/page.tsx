"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getEmployeeById } from '@/lib/employeeService'
import { Employee } from '@/types/employee';
import EmployeeUpdateDialog from '@/components/employees/EmployeeUpdateDialog';


function EditEmployeeDetails() {

    const params = useParams()
    const id = Number(params.id)
    const router = useRouter();
    const [employee, setEmployee] = useState<Employee | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
      const fetchEmployee = async()=>{
        try {
          const data = await getEmployeeById(id);
          setEmployee(data);
        } catch (error) {
          console.error("Error:", error);

        }finally{
          setLoading(false);
        }
      }
      fetchEmployee()
    }, [id]);
   
     if (loading) {
    return <div>Loading...</div>;
  }

  if (!employee) {
    return <div>Employee not found.</div>;
  }
  return (
    <EmployeeUpdateDialog
      employee={employee}
      open={true}
      onOpenChange={(open) => {
        if(!open){
          router.push('/employees'); // Navigate back to the employees list
        }
      }}
    />
  )
}

export default EditEmployeeDetails;