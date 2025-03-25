export const convertToHHMM = (decimal: number): string => {
    const hours = Math.floor(decimal);
    const minutes = Math.round((decimal - hours) * 60);
    return `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}`;
  };

export const convertToDecimal = (time: string): number => {
    const [hours, minutes] = time.split(":").map(Number);
    return hours + minutes / 60; // Convert minutes to fraction of an hour
  };

export const validateTimeFormat = (time: string) => {
  const timeRegex = /^([0-1]\d|2[0-3]):([0-5]\d)$/;
  const match = time.match(timeRegex);
    if (!match) return false; // Invalid format

    const [, hours, minutes] = match.map(Number);
    
    return hours * 60 + minutes >= 30;
};