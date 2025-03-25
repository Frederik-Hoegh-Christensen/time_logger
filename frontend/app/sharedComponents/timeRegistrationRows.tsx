import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import { convertToHHMM } from "~/utils/timeHelper";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencil, faTrash } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";
import ActionConfirmationDialog from "./ActionConfirmationDialog";
import NewEditModal from "./NewEditModal";

interface IProps {
  timeRegistrations: TimeRegistrationDTO[];
  onEdit: () => void;
  onDelete: (timeRegistrationId: string) => Promise<void>;
}

const TimeRegistrationRows: React.FC<IProps> = ({ timeRegistrations, onEdit, onDelete }) => {
  const [dialogOpenId, setDialogOpenId] = useState<string | null>(null);
  const [openEditModalId, setOpenEditModalId] = useState<string | null>(null);

  const handleDeleteClick = (timeRegistrationId: string) => {
    setDialogOpenId(timeRegistrationId);
  };

  const handleEditClick = (timeRegistrationId: string) => {
    setOpenEditModalId(timeRegistrationId)
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: "3px", marginBottom: 10 }}>
      {timeRegistrations.map((entry, index) => (
        <div
          key={index}
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "center",
            padding: "2px",
            border: "1px solid #ddd",
            borderRadius: "5px",
            backgroundColor: "#f9f9f9",
          }}
        >
          {entry.projectName !== "" && (
            <div style={{ margin: 10, width: 200 }}>
              <strong>Project:</strong> {entry.projectName}
            </div>
          )}
          <div style={{ margin: 10, width: 200 }}>
            <strong>Date:</strong> {entry.workDate}
          </div>
          <div style={{ margin: 10, width: 200 }}>
            <strong>Hours Worked:</strong> {convertToHHMM(entry.hoursWorked)}
          </div>
          <div style={{ margin: 10, width: 200 }}>
            <strong>Description:</strong> {entry.description}
          </div>
          <div>
            <button style={{ cursor: "pointer", padding: 10 }} onClick={() => setOpenEditModalId(entry.id)}>
              <FontAwesomeIcon icon={faPencil} />
            </button>
            <button style={{ cursor: "pointer", padding: 10 }} onClick={() => handleDeleteClick(entry.id)}>
              <FontAwesomeIcon icon={faTrash} />
            </button>
          </div>
          <ActionConfirmationDialog
            title="Are you sure you want to delete this time registration?"
            isOpen={dialogOpenId === entry.id}
            onClose={() => setDialogOpenId(null)}
            onConfirm={() => {
              onDelete(entry.id);
              setDialogOpenId(null);
            }}
          />
          <NewEditModal timeRegistration={entry} onConfirm={onEdit} onClose={() => console.log("")} open={openEditModalId === entry.id}/>
        </div>
      ))}
    </div>
  );
};

export default TimeRegistrationRows;
