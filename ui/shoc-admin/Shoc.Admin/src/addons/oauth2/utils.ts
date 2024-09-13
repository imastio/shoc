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

export function decodeJwt(jwtToken: string): any {

  if(!jwtToken){
    return null;
  }

  const arrayToken = jwtToken.split('.');

  if(arrayToken.length !== 3){
    return null;
  }

  try{
    return JSON.parse(Buffer.from(arrayToken[1], 'base64').toString());
  }
  catch(e){
    return null;
  }
}
