import React from 'react'
import Container from './Container';

function PageTitle({ text }: { text: string }) {
  return(
    <div className='py-8'>
      <Container>
        <div className='flex items-start bg-[()]'>
          <h1 className='uppercase font-bold'>{text}</h1>
        </div>
      </Container>
    </div>
  )
}

export default PageTitle