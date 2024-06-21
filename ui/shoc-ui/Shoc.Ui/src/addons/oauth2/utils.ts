export function mapExpiresAt(expiresAt: number | Date | string): Date {
    
    if (expiresAt instanceof Date) {
      return expiresAt;
    }
  
    // UNIX timestamp
    if (typeof expiresAt === 'number') {
      return new Date(expiresAt * 1000);
    }
  
    // ISO 8601 string
    return new Date(expiresAt);
}


export function mapExpiresIn(expiresIn: number | string): Date {
    return new Date(Date.now() + Number.parseInt(expiresIn.toString(), 10) * 1000);
}