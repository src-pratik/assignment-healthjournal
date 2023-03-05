export interface EditionListDTO {
    id: string;
    journalName: string;
    journalId: string;
    createdOn: string;
}

export interface Login {
    username: string;
    password: string;
}

export interface TokenResponse {
    token: string;
}

export class JournalDTO {
    public id!: string;
    public name!: string;
    constructor(
    ) { }
  }
  
  export class JournalDetailsDTO {
    public id!: string;
    public name!: string;
    public createdBy!: string;
    public createdOn!: string;
    constructor(
    ) { }
  }