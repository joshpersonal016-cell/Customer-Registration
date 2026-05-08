export interface Customer {
    id?: string
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
    signatureUrl?: string
    createdAt?: string
    updatedAt?: string
}

export interface CreateCustomerDto {
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
    signature?: Blob | null
}

type CustomerForm = {
  firstName: string
  lastName: string
  email: string
  phoneNumber: string

  signatureUrl: string | null
  signatureBlob: Blob | null
}

export type Props = {
  visible: boolean
  onClose: () => void
  onSubmit?: () => void
  formData: CustomerForm
  setFormData: React.Dispatch<React.SetStateAction<CustomerForm>>
  readOnly?: boolean
}
