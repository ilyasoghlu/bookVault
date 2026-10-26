"use client"

import React from 'react'
import { BookCase } from '@/types/bookcase';
import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import Link from 'next/link';
import { useAuth } from '@/hooks/useAuth';
import DeleteBookCaseButton from './DeleteBookCaseButton';

import {ToolCase} from "lucide-react"

interface BookCaseProps{
    bookCase:BookCase;
    onDelete:(id:number) => void
}

function CaseCard({bookCase, onDelete}: BookCaseProps) {
  const {hasPermission} = useAuth();
  return (
    <Card>
      <CardHeader>
        <CardTitle>
            <ToolCase />     
            {bookCase.casenumber}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Case Number: {bookCase.casenumber}</strong>
        </p>
        <p>
            <strong>Capacity: {bookCase.capacity}</strong>
        </p>
        <p className='mt-2'></p>
          <Link href={`/bookcases/details/${bookCase.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("BOOKCASE_UPDATE")&&(
            <Link href={`/bookcases/edit/${bookCase.id}`}>
              <Button>
                Edit Case
              </Button>
            </Link>
          )}
          {hasPermission("BOOKCASE_DELETE")&&(
            <DeleteBookCaseButton
            id={bookCase.id} 
            onDelete={onDelete}
            // fullname={`${bookCase.id}`} 
            />
          )}
      </CardContent>
    </Card>
  )
}

export default CaseCard
