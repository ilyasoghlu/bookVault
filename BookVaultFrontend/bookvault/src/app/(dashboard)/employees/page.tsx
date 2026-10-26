"use client"


import { useEffect, useState } from "react"
import { getEmployees } from "@/lib/employeeService"
import { Employee } from "@/types/employee"
import EmployeeCard from "@/components/employees/EmployeeCard"
import DataView from "@/components/data-view/DataView"
import DeleteEmployeeButton from "@/components/employees/DeleteEmployeeButton"
import { Button } from "@/components/ui/button"
import { useAuth } from "@/hooks/useAuth"
import Link from "next/link"
import { DataViewConfig } from "@/components/data-view/types"


function EmployeePage() {

  const [employee, setEmployee] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null); 
  const {hasPermission} = useAuth();
  const totalEmployees = employee.length;

  const employeeConfig:DataViewConfig<Employee> = {
    columns: [
      {
        key: "firstname",
        label: "First name",
      },
      {
        key: "lastname",
        label:"Last name",
      },
      {
        key:"employeeposition",
        label:"Position",
      },
      {
        key:"id",
        label:"Actions",
        render:(employee: Employee) => (
           <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/employees/details/${employee.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("EMPLOYEE_UPDATE") && (
        <Link href={`/employees/edit/${employee.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("EMPLOYEE_DELETE") && (
        <DeleteEmployeeButton
          id={employee.id}
          onDelete={handleDelete}
        />
      )}
    </div>
        )
      }
    ]
  }


  const handleDelete = (id:number) =>{
    setEmployee((prev)=> prev.filter((employee) => employee.id !==id))
  }

   useEffect(() => {
      const fetchEmployees = async () => {
        try {
          const data = await getEmployees();
          setEmployee(data);
        } catch (error) {
          console.error("Error:", error);
          setError("Employees' information cant found.")
        }finally{
          setLoading(false)
  
        };
      };
       fetchEmployees();
  }, []);


   if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }

  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalEmployees} Employee{totalEmployees> 1 && 's'} found</h4>
        {hasPermission("EMPLOYEE_CREATE")&&(
          <button
            type="button"
            className="
              rounded-md 
              outline 
              hover:outline-blue-300
              hover:text-orange-600
              border 
              px-3 
              py-2 
              text-sm"
            >
            <Link href="/employees/create">
              Create Employee
            </Link>
          </button>
        )}
      </div>
    
     {employee.length === 0 ?  (
        <p>No employee found.</p>
     ):(
      <DataView
        data={employee}
        config ={employeeConfig}
        renderGridItem={(employee) =>(
          <EmployeeCard 
            employee={employee}
            onDelete={handleDelete}
          />
        )}
      />
     )}
    
    </div>
  )
}

export default EmployeePage





