import Popup from 'devextreme-react/popup'
import Form, {
  Item,
  GroupItem,
  Label,
  RequiredRule,
  EmailRule,
  PatternRule,
} from 'devextreme-react/form'
import Button from 'devextreme-react/button'
import SignatureCanvas from 'react-signature-canvas'
import { useEffect, useRef } from 'react'

type CustomerForm = {
  firstName: string
  lastName: string
  email: string
  phoneNumber: string

  signatureUrl: string | null
  signatureBlob: Blob | null
}

type Props = {
  visible: boolean
  onClose: () => void
  onSubmit?: () => void
  formData: CustomerForm
  setFormData: React.Dispatch<React.SetStateAction<CustomerForm>>
  readOnly?: boolean
}

export default function CustomerModal({
  visible,
  onClose,
  onSubmit,
  formData,
  setFormData,
  readOnly,
}: Props) {
  const sigRef = useRef<SignatureCanvas | null>(null)
  const formRef = useRef<any>(null)

  const isViewMode = readOnly === true

  // reset signature when opening edit mode
  useEffect(() => {
    if (visible && !isViewMode && sigRef.current) {
      sigRef.current.clear()
    }
  }, [visible, isViewMode])

  const handleChange = (e: any) => {
    if (isViewMode) return

    setFormData((prev) => ({
      ...prev,
      [e.dataField]: e.value,
    }))
  }

  // 🖊️ capture signature blob
  const handleSignatureEnd = () => {
    if (isViewMode) return
    if (!sigRef.current) return

    sigRef.current.getTrimmedCanvas().toBlob((blob) => {
      if (!blob) return

      setFormData((prev) => ({
        ...prev,
        signatureBlob: blob,
      }))
    }, 'image/png')
  }

  const clearSignature = () => {
    if (isViewMode) return

    sigRef.current?.clear()

    setFormData((prev) => ({
      ...prev,
      signatureBlob: null,
    }))
  }

  // 📌 signature validation
  const isSignatureValid = () => {
    return !!formData.signatureBlob || !!formData.signatureUrl
  }

  return (
    <Popup
      visible={visible}
      onHiding={onClose}
      showCloseButton
      hideOnOutsideClick
      dragEnabled
      showTitle
      title={isViewMode ? 'Customer Details' : 'Add Customer'}
      width={526}
      height={650}
    >
      <div style={{ padding: 20, background: '#fff', borderRadius: 10 }}>
        <Form
          ref={formRef}
          formData={formData}
          onFieldDataChanged={handleChange}
          colCount={2}
          labelLocation="top"
          readOnly={isViewMode}
        >
          <GroupItem colSpan={2} caption="Customer Information">

            {/* FIRST NAME */}
            <Item dataField="firstName">
              <Label text="First Name" />
              <RequiredRule message="First name is required" />
            </Item>

            {/* LAST NAME */}
            <Item dataField="lastName">
              <Label text="Last Name" />
              <RequiredRule message="Last name is required" />
            </Item>

            {/* EMAIL */}
            <Item dataField="email">
              <Label text="Email" />
              <RequiredRule message="Email is required" />
              <EmailRule message="Invalid email format" />
            </Item>

            {/* PHONE */}
            <Item dataField="phoneNumber">
              <Label text="Phone Number" />
              <RequiredRule message="Phone number is required" />
              <PatternRule
                pattern={/^(09|\+639)\d{9}$/}
                message="Invalid phone number format"
              />
            </Item>

            {/* SIGNATURE */}
            <Item
              dataField="signature"
              colSpan={2}
              label={{ text: 'Signature' }}
              render={() => (
                <div>
                  {/* VIEW MODE */}
                  {isViewMode ? (
                    formData.signatureUrl ? (
                      <img
                        src={formData.signatureUrl}
                        alt="signature"
                        style={{
                          width: '100%',
                          border: '1px solid #ddd',
                          borderRadius: 8,
                          background: '#fafafa',
                          pointerEvents: 'none',
                        }}
                      />
                    ) : (
                      <div
                        style={{
                          height: 80,
                          border: '1px solid #ddd',
                          borderRadius: 8,
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          color: '#aaa',
                          fontSize: 13,
                        }}
                      >
                        No signature provided
                      </div>
                    )
                  ) : (
                    /* EDIT MODE */
                    <>
                      <div
                        style={{
                          border: '1px solid #ddd',
                          borderRadius: 8,
                          background: '#fff',
                        }}
                      >
                        <SignatureCanvas
                          ref={sigRef}
                          penColor="black"
                          canvasProps={{
                            width: 500,
                            height: 160,
                            style: {
                              width: '100%',
                              height: 160,
                              touchAction: 'none',
                            },
                          }}
                          onEnd={handleSignatureEnd}
                        />
                      </div>

                      <button
                        type="button"
                        onClick={clearSignature}
                        style={{
                          marginTop: 8,
                          padding: '6px 10px',
                          cursor: 'pointer',
                        }}
                      >
                        Clear Signature
                      </button>

                      {/* signature validation */}
                      {!isSignatureValid() && (
                        <div style={{ color: 'red', fontSize: 12, marginTop: 5 }}>
                          Signature is required
                        </div>
                      )}
                    </>
                  )}
                </div>
              )}
            />

          </GroupItem>
        </Form>

        {/* BUTTONS */}
        {!isViewMode && (
          <div
            style={{
              marginTop: 25,
              display: 'flex',
              justifyContent: 'flex-end',
              gap: 10,
            }}
          >
            <Button text="Cancel" onClick={onClose} />

            <Button
              text="Save Customer"
              type="default"
              onClick={() => {
                const validation = formRef.current?.instance.validate()

                if (!validation?.isValid || !isSignatureValid()) {
                  return
                }

                onSubmit?.()
              }}
            />
          </div>
        )}
      </div>
    </Popup>
  )
}
