export const getCurrentWeekDates = (): Date[] => {
    const today = new Date();
    const dayOfWeek = today.getUTCDay(); // 0 (Sunday) to 6 (Saturday)
  
    // Find last Monday
    const lastMonday = new Date(today);
    lastMonday.setUTCDate(today.getUTCDate() - (dayOfWeek === 0 ? 6 : dayOfWeek - 1));
  
    // Generate the full week (Monday - Sunday)
    const weekDates: Date[] = [];
    for (let i = 0; i < 7; i++) {
      const date = new Date(lastMonday);
      date.setUTCDate(lastMonday.getUTCDate() + i);
      weekDates.push(date);
    }
  
    return weekDates;
  };

export const getMondayOfWeek = (date: Date): Date => {
    const monday = new Date(date);
    const dayOfWeek = monday.getUTCDay();
    const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek; // Adjust for Sunday
    monday.setUTCDate(monday.getUTCDate() + diff);
    return monday;
  };
  
  // Helper function to get the week number
export const getWeekNumber = (date: Date): number => {
    const firstJan = new Date(date.getUTCFullYear(), 0, 1);
    const days = Math.floor((date.getTime() - firstJan.getTime()) / (24 * 60 * 60 * 1000));
    return Math.ceil((days + firstJan.getUTCDay() + 1) / 7);
  };