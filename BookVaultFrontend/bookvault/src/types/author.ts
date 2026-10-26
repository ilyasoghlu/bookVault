

export interface Author {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: string;
  deathdate?: string | null;
  nationality: string;
  biography: string;
  createdat?: string | null;
  updatedat?: string | null;
  publisherid?: string | null;
}

export interface CreateAuthorRequest {
  firstname: string;
  lastname: string;
  birthdate: string;
  deathdate?: string | null;
  nationality: string;
  biography: string;
  publisherid?: string | null;
}
export interface UpdateAuthorRequest {
  firstname: string;
  lastname: string;
  birthdate: string;
  deathdate?: string | null;
  nationality: string;
  biography: string;
  publisherid?: string | null;
}