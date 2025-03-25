
import type { TimeRegistrationCreateDTO, TimeRegistrationDTO } from "~/models/timeRegistration";
import { apiService } from "./apiService";

export const timeRegistrationService = {
  fetchTimeRegistrationsByProjectId: async (timeRegistrationId: string): Promise<TimeRegistrationDTO[]> => {
    return await apiService.get<TimeRegistrationDTO[]>(`/timeRegistration/project/${timeRegistrationId}`);
  },
  fetchTimeRegistrationsByFreelancerIdAndDate: async (freelancerId: string, date: Date): Promise<TimeRegistrationDTO[]> => {
    const formattedDate = date.toISOString().split("T")[0];
    return await apiService.get<TimeRegistrationDTO[]>(`/timeRegistration/freelancer/${freelancerId}/date/${formattedDate}`);
  },

  createTimeRegistration: async (timeRegistrationData: TimeRegistrationCreateDTO): Promise<void> => {
    return await apiService.post('/timeRegistration', timeRegistrationData);
  },

  updateTimeRegistration: async (timeRegistrationId: string, updatedTimeRegistration: TimeRegistrationDTO): Promise<void> => {
    return await apiService.put(`/TimeRegistration/${timeRegistrationId}`, updatedTimeRegistration);
  },

  deleteTimeRegistration: async (timeRegistrationId: string): Promise<void> => {
    return await apiService.delete(`/timeRegistration/${timeRegistrationId}`);
  },
};

