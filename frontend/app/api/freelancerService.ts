import type { FreelancerCreateDTO, FreelancerDTO } from "~/models/freelancer";
import { apiService } from "./apiService";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import type { ProjectDTO } from "~/models/project";

export const freelancerService = {
  // Get Freelancer by ID
  getFreelancer: async (id: string) => {
    return await apiService.get<FreelancerDTO>(`/freelancers/${id}`);
  },

  // Get time registrations by Freelancer ID and date
  getTimeRegistrationsByFreelancerIdAndDate: async (freelancerId: string, date: string) => {
    return await apiService.get<TimeRegistrationDTO[]>(
      `/freelancers/${freelancerId}/timeregistrations?date=${date}`
    );
  },

  // Get projects by Freelancer ID
  getProjectsByFreelancerId: async (freelancerId: string) => {
    return await apiService.get<ProjectDTO[]>(`/freelancers/${freelancerId}/projects`);
  },

  // Create a new Freelancer
  createFreelancer: async (freelancerData: FreelancerCreateDTO) => {
    return await apiService.post("/freelancers", freelancerData);
  },

  // Update an existing Freelancer
  updateFreelancer: async (id: string, freelancerData: FreelancerDTO) => {
    return await apiService.put(`/freelancers/${id}`, freelancerData);
  },

  // Delete a Freelancer by ID
  deleteFreelancer: async (id: string) => {
    return await apiService.delete(`/freelancers/${id}`);
  },
};
