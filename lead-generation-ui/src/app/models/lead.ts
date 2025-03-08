export interface Lead {
    id: number;
    name: string;
    profileUrl: string;
    jobTitle: string;
    companyName: string;
    emailAddress: string;
    createdAt: Date;
    updatedAt: Date;
    userId: number;
}

export interface ScoredLead extends Lead {
    industryMatch: number;
    jobTitleMatch: number;
    connections: number;
    score: number;
} 