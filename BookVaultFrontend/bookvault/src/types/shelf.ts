
import { Section } from "./section";
import { BookCase } from "./bookcase";
import { Room } from "./room";

export interface Shelf {
    id: number;
    shelvecode: number;
    capacity: number;

    sectionid?: number | null;
    caseid?: number | null;
    roomid?: number | null;

    createdat?: string | null;
    updatedat?: string | null;

    section?: Section | null;
    case?: BookCase | null;
    room?: Room | null;
}

interface CreateShelfPayload {
    shelvecode: number;
    capacity: number;
    caseid: number;
}

interface UpdateShelfPayload {
    shelvecode: number;
    capacity: number;
}

export type CreateShelfRequest = CreateShelfPayload;
export type UpdateShelfRequest = UpdateShelfPayload;

